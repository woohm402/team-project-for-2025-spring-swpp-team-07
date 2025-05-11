using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using Mapbox.Map;
using System.Collections;
using System;
using Dreamteck.Splines;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralRoad : MonoBehaviour
{
    [Header("맵 설정")]
    public AbstractMap map;

    [Header("OSM GeoJSON")]
    public TextAsset geoJson;

    [Header("도로 설정")]
    public float roadWidth = 6f;
    public float maxSegmentLength = 10f;
    public Material roadMaterial;

    [Header("스플라인 설정")]
    public bool useSpline = false;

    Mesh _mesh;
    private bool _isMeshBuilt = false;

#if UNITY_EDITOR
    [ContextMenu("도로 프리팹으로 저장")]
    void SaveAsPrefab()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null)
        {
            Debug.LogError("프리팹 저장 불가: 메시 필터 또는 생성된 메시가 없습니다.");
            return;
        }

        Mesh originalMesh = mf.sharedMesh;

        if (string.IsNullOrEmpty(originalMesh.name))
        {
            originalMesh.name = "ProceduralRoadMeshInstance";
        }

        string prefabDirectoryPath = "Assets/Prefabs";
        string meshDirectoryPath = Path.Combine(prefabDirectoryPath, "Meshes");

        void EnsureDirectoryExistsLocal(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string parentPath = Path.GetDirectoryName(path);
                string folderName = Path.GetFileName(path);

                if (!string.IsNullOrEmpty(parentPath) && parentPath != "Assets" && !AssetDatabase.IsValidFolder(parentPath))
                {
                    EnsureDirectoryExistsLocal(parentPath);
                }

                if (!string.IsNullOrEmpty(folderName))
                {
                    try
                    {
                        string guid = AssetDatabase.CreateFolder(parentPath, folderName);
                        if (string.IsNullOrEmpty(guid))
                        {
                            throw new IOException($"AssetDatabase.CreateFolder 실패: {path}");
                        }
                        Debug.Log($"디렉토리 생성: {path}");
                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"디렉토리 생성 실패 '{path}': {e.Message}");
                        throw;
                    }
                }
            }
        }

        try
        {
            EnsureDirectoryExistsLocal(prefabDirectoryPath);
            EnsureDirectoryExistsLocal(meshDirectoryPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"디렉토리 생성 오류로 프리팹 저장 중단: {e.Message}");
            return;
        }

        Mesh meshToSave = Instantiate(originalMesh);
        meshToSave.name = originalMesh.name;

        string meshPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(meshDirectoryPath, $"{meshToSave.name}.asset"));
        try
        {
            AssetDatabase.CreateAsset(meshToSave, meshPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"메시 에셋 저장 경로: {meshPath}", meshToSave);
        }
        catch (Exception e)
        {
            Debug.LogError($"메시 에셋 저장 오류: {e.Message}\n{e.StackTrace}");
            return;
        }

        mf.sharedMesh = meshToSave;

        string prefabPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(prefabDirectoryPath, $"{this.gameObject.name}_Road.prefab"));
        try
        {
            GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(this.gameObject, prefabPath);

            if (savedPrefab != null)
            {
                Debug.Log($"도로 프리팹 저장 경로: {prefabPath}", savedPrefab);
                EditorGUIUtility.PingObject(savedPrefab);
                EditorGUIUtility.PingObject(meshToSave);
            }
            else
            {
                Debug.LogError($"프리팹 저장 실패: {prefabPath}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"프리팹 저장 오류: {e.Message}\n{e.StackTrace}");
        }
    }

    [ContextMenu("스플라인 프리팹으로 저장")]
    void SaveSplinesAsPrefab()
    {
        var splineComps = GetComponentsInChildren<SplineComputer>();
        if (splineComps == null || splineComps.Length == 0)
        {
            Debug.LogError("저장할 스플라인이 없습니다.");
            return;
        }
        string prefabDirectoryPath = "Assets/Prefabs";
        void EnsureDirectoryExistsLocal(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string parentPath = Path.GetDirectoryName(path);
                string folderName = Path.GetFileName(path);
                if (!string.IsNullOrEmpty(parentPath) && parentPath != "Assets" && !AssetDatabase.IsValidFolder(parentPath))
                    EnsureDirectoryExistsLocal(parentPath);
                if (!string.IsNullOrEmpty(folderName))
                {
                    AssetDatabase.CreateFolder(parentPath, folderName);
                    AssetDatabase.Refresh();
                }
            }
        }
        try { EnsureDirectoryExistsLocal(prefabDirectoryPath); } catch (Exception e)
        { Debug.LogError($"디렉토리 생성 오류로 스플라인 프리팹 저장 중단: {e.Message}"); return; }
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(prefabDirectoryPath, $"{this.gameObject.name}_Splines.prefab"));
        try
        {
            var savedPrefab = PrefabUtility.SaveAsPrefabAsset(this.gameObject, prefabPath);
            if (savedPrefab != null)
            {
                Debug.Log($"스플라인 프리팹 저장 경로: {prefabPath}", savedPrefab);
                EditorGUIUtility.PingObject(savedPrefab);
            }
            else Debug.LogError($"스플라인 프리팹 저장 실패: {prefabPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"스플라인 프리팹 저장 오류: {e.Message}\n{e.StackTrace}");
        }
    }
#endif

    void Awake()
    {
        if (map != null)
        {
            map.OnInitialized += HandleMapInitialized;
        }
        else
        {
            Debug.LogError("AbstractMap이 할당되지 않았습니다.");
        }
    }

    void OnDestroy()
    {
        if (map != null)
        {
            map.OnInitialized -= HandleMapInitialized;
        }
    }

    void HandleMapInitialized()
    {
        Debug.Log("맵 초기화 완료. 도로 메시 생성을 위해 10초 대기합니다.");
        if (map.MapVisualizer != null)
        {
            StartCoroutine(DelayedBuild(10.0f));
        }
        else
        {
            Debug.LogError("OnInitialized 후에도 MapVisualizer가 null입니다. 메시를 빌드할 수 없습니다.");
        }
    }

    IEnumerator DelayedBuild(float delay)
    {
        Debug.Log($"{delay}초 대기 중...");
        yield return new WaitForSeconds(delay);
        Debug.Log("대기 완료. 도로 메시 빌드를 시도합니다.");
        BuildRoadMesh();
    }

    void BuildRoadMesh()
    {
        if (useSpline)
        {
            CreateSplines();
            return;
        }

        if (_isMeshBuilt)
        {
            Debug.Log("BuildRoadMesh 호출되었지만 메시가 이미 빌드되었습니다 (_isMeshBuilt=true). 건너뜁니다.");
            return;
        }

        if (map == null || map.MapVisualizer == null)
        {
            Debug.LogWarning("BuildRoadMesh 호출되었지만 맵 또는 MapVisualizer가 준비되지 않았습니다. 도로 메시를 빌드할 수 없습니다.");
            return;
        }

        Debug.Log("지연 후 BuildRoadMesh 프로세스 시작 중...");

        if (_mesh != null) _mesh.Clear();
        else _mesh = new Mesh { name = "ProceduralRoadMesh" };

        var verts = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>();
        int vertOffset = 0;

        JObject json;
        try
        {
            json = JObject.Parse(geoJson.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"GeoJSON 파싱 실패: {e.Message}");
            return;
        }
        var features = json["features"] as JArray;
        if (features == null)
        {
            Debug.LogError("GeoJSON에 'features' 배열이 없습니다.");
            return;
        }

        foreach (var featToken in features)
        {
            var feat = featToken as JObject;
            if (feat?["geometry"]?["type"]?.Value<string>() != "LineString") continue;
            var coordArr = feat["geometry"]?["coordinates"] as JArray;
            if (coordArr == null || coordArr.Count < 2) continue;

            float featureWidth = roadWidth;
            var lanesToken = feat["properties"]?["lanes"];
            if (lanesToken != null)
            {
                int lanesValue = 0;
                if (lanesToken.Type == JTokenType.Integer || lanesToken.Type == JTokenType.Float)
                {
                    double tmp = lanesToken.Value<double>();
                    lanesValue = (int)Math.Round(tmp);
                }
                else
                {
                    int.TryParse(lanesToken.ToString(), out lanesValue);
                }
                Debug.Log($"차선 파싱: 토큰='{lanesToken}', 파싱된 값={lanesValue}");
                if (lanesValue == 4)
                {
                    featureWidth = roadWidth * 2f;
                    Debug.Log("4차선 감지, 너비 두 배 증가.");
                }
            }

            var worldPtsXZ = new List<Vector3>();
            foreach (var pToken in coordArr)
            {
                var p = pToken as JArray;
                if (p != null && p.Count >= 2)
                {
                    try
                    {
                        double lon = p[0].Value<double>();
                        double lat = p[1].Value<double>();
                        var wp = map.GeoToWorldPosition(new Vector2d(lat, lon), false);
                        worldPtsXZ.Add(wp);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"GeoJSON의 잘못된 좌표 건너뜁니다: {pToken}. 오류: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"GeoJSON의 잘못된 좌표 형식 건너뜁니다: {pToken}");
                }
            }
            if (maxSegmentLength > 0f)
            {
                var subdividedPts = new List<Vector3>();
                if (worldPtsXZ.Count > 0) subdividedPts.Add(worldPtsXZ[0]);
                for (int k = 1; k < worldPtsXZ.Count; k++)
                {
                    var p0 = worldPtsXZ[k - 1];
                    var p1 = worldPtsXZ[k];
                    float dist = Vector3.Distance(p0, p1);
                    if (dist > maxSegmentLength)
                    {
                        float traveled = maxSegmentLength;
                        while (traveled < dist)
                        {
                            float t = traveled / dist;
                            subdividedPts.Add(Vector3.Lerp(p0, p1, t));
                            traveled += maxSegmentLength;
                        }
                    }
                    subdividedPts.Add(p1);
                }
                worldPtsXZ = subdividedPts;
            }

            if (worldPtsXZ.Count < 2) continue;

            for (int i = 0; i < worldPtsXZ.Count; i++)
            {
                Vector3 forward;
                if (i == 0) forward = (worldPtsXZ[1] - worldPtsXZ[0]).normalized;
                else if (i == worldPtsXZ.Count - 1) forward = (worldPtsXZ[i] - worldPtsXZ[i - 1]).normalized;
                else
                {
                    var dirA = (worldPtsXZ[i] - worldPtsXZ[i - 1]).normalized;
                    var dirB = (worldPtsXZ[i + 1] - worldPtsXZ[i]).normalized;
                    forward = (dirA + dirB);
                    if (forward.sqrMagnitude < 0.001f)
                    {
                        forward = Vector3.Cross(dirA, Vector3.up);
                    }
                    forward.Normalize();
                }

                if (forward == Vector3.zero)
                {
                    Debug.LogWarning($"인덱스 {i}에서 전방 벡터가 0입니다. 가능한 경우 이전 법선 사용.");
                    if (i > 0)
                    {
                        vertOffset += 2;
                    }
                    continue;
                }

                var rightNormal = Vector3.Cross(forward, Vector3.up).normalized;
                float halfW = featureWidth * 0.5f;
                var centerPosXZ = worldPtsXZ[i];

                var vL_XZ = centerPosXZ - rightNormal * halfW;
                var vR_XZ = centerPosXZ + rightNormal * halfW;

                var geoL = map.WorldToGeoPosition(vL_XZ);
                var geoR = map.WorldToGeoPosition(vR_XZ);

                float elevationL = map.QueryElevationInUnityUnitsAt(geoL);
                float elevationR = map.QueryElevationInUnityUnitsAt(geoR);

                Vector3 vL = new Vector3(vL_XZ.x, elevationL, vL_XZ.z);
                verts.Add(vL);

                Vector3 vR = new Vector3(vR_XZ.x, elevationR, vR_XZ.z);
                verts.Add(vR);

                float distanceSegment = (i == 0) ? 0f : Vector3.Distance(worldPtsXZ[i], worldPtsXZ[i - 1]);
                float previousU = (uvs.Count > 1) ? uvs[uvs.Count - 2].x : 0f;
                float textureScaleAlongLength = 1.0f / featureWidth;
                float uCoord = previousU + (distanceSegment * textureScaleAlongLength);

                uvs.Add(new Vector2(uCoord, 0));
                uvs.Add(new Vector2(uCoord, 1));

                if (i > 0)
                {
                    int baseIndex = vertOffset - 2;
                    if (baseIndex >= 0 && (baseIndex + 3) < verts.Count)
                    {
                        tris.Add(baseIndex + 0);
                        tris.Add(baseIndex + 1);
                        tris.Add(baseIndex + 2);

                        tris.Add(baseIndex + 1);
                        tris.Add(baseIndex + 3);
                        tris.Add(baseIndex + 2);
                    }
                    else
                    {
                        Debug.LogError($"세그먼트 {i}에서 삼각형 생성에 대한 잘못된 인덱스. BaseIndex: {baseIndex}, Verts Count: {verts.Count}");
                    }
                }
                vertOffset += 2;
            }
        }

        if (verts.Count < 4 || tris.Count < 6)
        {
            Debug.LogWarning($"유효한 메시를 생성하기에 정점({verts.Count}) 또는 삼각형({tris.Count})이 충분하지 않습니다. GeoJSON이 너무 짧거나 잘못되었을 수 있습니다.");
            var mfClear = GetComponent<MeshFilter>();
            if (mfClear.sharedMesh != null) mfClear.sharedMesh = null;
            _isMeshBuilt = false;
            return;
        }

        Debug.Log($"{verts.Count}개의 정점과 {tris.Count}개의 삼각형으로 도로 메시 생성을 진행합니다.");

        _mesh.SetVertices(verts);
        _mesh.SetUVs(0, uvs);
        _mesh.SetTriangles(tris, 0);

        try
        {
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
        }
        catch (Exception e)
        {
            Debug.LogError($"메시 후처리(RecalculateNormals/Bounds) 중 오류: {e.Message}");
            _isMeshBuilt = false;
            return;
        }

        var mf = GetComponent<MeshFilter>();
        mf.sharedMesh = _mesh;
        var mr = GetComponent<MeshRenderer>();
        mr.sharedMaterial = roadMaterial;

        _isMeshBuilt = true;
        Debug.Log("지연 후 도로 메시가 성공적으로 생성되었습니다.");
    }

    void CreateSplines()
    {
        foreach (var existing in GetComponentsInChildren<SplineComputer>()) DestroyImmediate(existing.gameObject);
        JObject json;
        try { json = JObject.Parse(geoJson.text); } catch (Exception e) { Debug.LogError($"GeoJSON 파싱 실패: {e.Message}"); return; }
        var features = json["features"] as JArray;
        if (features == null) { Debug.LogError("GeoJSON에 'features' 배열이 없습니다."); return; }
        int idx = 0;
        foreach (var featToken in features)
        {
            var feat = featToken as JObject;
            if (feat?["geometry"]?["type"]?.Value<string>() != "LineString") continue;
            var coordArr = feat["geometry"]?["coordinates"] as JArray;
            if (coordArr == null || coordArr.Count < 2) continue;
            var pts = new List<Vector3>();
            foreach (var pToken in coordArr)
            {
                var p = pToken as JArray;
                if (p != null && p.Count >= 2)
                {
                    double lon = p[0].Value<double>(); double lat = p[1].Value<double>();
                    var wp = map.GeoToWorldPosition(new Vector2d(lat, lon), false);
                    var geo = map.WorldToGeoPosition(wp);
                    float elev = map.QueryElevationInUnityUnitsAt(geo);
                    pts.Add(new Vector3(wp.x, elev, wp.z));
                }
            }
            if (pts.Count < 2) continue;
            var splineGO = new GameObject($"Spline_{idx++}");
            splineGO.transform.SetParent(transform, false);
            var spline = splineGO.AddComponent<SplineComputer>();
            var points = new SplinePoint[pts.Count];
            for (int i = 0; i < pts.Count; i++)
            {
                points[i] = new SplinePoint() { position = pts[i], normal = Vector3.up, size = 1f, color = Color.white };
            }
            spline.SetPoints(points);
        }
        _isMeshBuilt = true;
        Debug.Log("스플라인이 성공적으로 생성되었습니다.");
    }
}
