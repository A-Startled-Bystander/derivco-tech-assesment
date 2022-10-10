using Roulette.Controllers;
using Roulette.Model;

namespace RouletteTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SpinWheelTest()
        {
            // Arrange
            var controller = new RouletteController();

            // Act
            var test = controller.SpinWheel();
            string result = test.Result.Value.ToString();
            string numberString = result.Substring(result.Length - 2, 2);
            numberString = String.Concat(numberString.Where(c => !Char.IsWhiteSpace(c)));
            int number = Int32.Parse(numberString);

            // Assert     
            Assert.LessOrEqual(number, 36);
            Assert.Greater(number, -1);
        }


        [Test]
        public void PlaceBetTest()
        {
            // Arrange
            var controller = new RouletteController();
            Bet obj = new Bet();
            obj.Numbers = new int[] { 1, 2, 3 };
            obj.Color = "red";
            obj.Amount = 10;
            obj.PlayerName = "RandomName";

            // Act
            var test = controller.PlaceBet(obj);
            int result = Int32.Parse(test.Result.StatusCode.ToString());

            // Assert     
            Assert.AreEqual(result, 200);
        }

        [Test]
        public void SpinTest()
        {
            // designed to fail since no spins history is added to the static class, error message is displayed from global exception handler

            // Arrange
            var controller = new RouletteController();

            // Act
            var test = controller.ShowPreviousSpins();
            int result = Int32.Parse(test.Result.StatusCode.ToString());

            // Assert     
            Assert.AreEqual(result, 200);
        }
    }
}