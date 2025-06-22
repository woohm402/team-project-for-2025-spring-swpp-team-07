using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SchoolRush.Assets.Scripts.Utils.Tests
{
    [TestFixture]
    public class RandomPickerTests
    {
        [Test]
        public void Pick_ShouldReturnRequestedCount()
        {
            // Arrange
            var items = new List<int> { 1, 2, 3, 4, 5 };
            var picker = new RandomPicker<int>(items);

            // Act
            var result = picker.pick(3);

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void Pick_ShouldNotDuplicate()
        {
            // Arrange
            var items = new List<int> { 1, 2, 3, 4, 5 };
            var picker = new RandomPicker<int>(items);

            // Act
            var result = picker.pick(3);

            // Assert
            Assert.That(result[0], Is.Not.EqualTo(result[1]));
            Assert.That(result[1], Is.Not.EqualTo(result[2]));
            Assert.That(result[2], Is.Not.EqualTo(result[0]));
        }

        [Test]
        public void Pick_ShouldUniform()
        {
            // Arrange
            var items = new List<int> { 1, 2, 3, 4, 5 };
            var picker = new RandomPicker<int>(items);
            var counts = new Dictionary<int, int>();

            for (int i = 0; i < 10000; i++) {
                var result = picker.pick(3);
                foreach (var item in result) {
                    if (counts.ContainsKey(item))
                        counts[item]++;
                    else
                        counts[item] = 1;
                }
            }

            foreach (var count in counts.Values) {
                Assert.That(count, Is.GreaterThan(5700).And.LessThan(6300));
            }
        }

        [Test]
        public void Pick_ShouldWorkAnysize()
        {
            // Arrange
            var items = new List<int> { 10 };
            var picker = new RandomPicker<int>(items);

            var result = picker.pick(1);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(10));
        }
    }
}
