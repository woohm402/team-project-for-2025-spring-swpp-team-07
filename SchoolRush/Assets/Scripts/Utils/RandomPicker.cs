using System.Collections.Generic;

public class RandomPicker<T> {
    private List<T> items;

    public RandomPicker(List<T> items) {
        this.items = new List<T>(items);
    }

    public List<T> pick(int count) {
        List<T> temp = new List<T>(items);
        List<T> ret = new List<T>();

        System.Random random = new System.Random();

        int remainingCount = count < temp.Count ? count : temp.Count;

        while (remainingCount > 0 && temp.Count > 0) {
            int randomIndex = random.Next(0, temp.Count);
            ret.Add(temp[randomIndex]);
            temp.RemoveAt(randomIndex);
            remainingCount--;
        }

        return ret;
    }
}
