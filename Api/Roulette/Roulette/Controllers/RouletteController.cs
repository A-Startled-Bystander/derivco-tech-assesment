using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Roulette.Data;
using Roulette.Model;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text.Json.Serialization;

namespace Roulette.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        [HttpPost]
        [Route("PlaceBet")]
        public async Task<OkObjectResult> PlaceBet(Bet betInput)
        {

            using (var db = new SQLiteConnection("Data Source=roulette.db;Version=3;New=True;Compress=True;"))
            {
                try
                {
                    db.Open();

                    SQLiteCommand checkTableExists = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='BETS';", db);
                    SQLiteDataReader reader = checkTableExists.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        //create table if no rows exist
                        SQLiteCommand createTable = new SQLiteCommand("CREATE TABLE BETS (Numbers NVARCHAR(100), Color NVARCHAR(10), Amount int, PlayerName NVARCHAR(50))", db);
                        createTable.ExecuteNonQuery();
                    }

                    SQLiteCommand insertData = new SQLiteCommand($"INSERT INTO BETS (Numbers, Color , Amount , PlayerName) VALUES ( '{string.Join(",", betInput.Numbers)}' , '{betInput.Color}' , {betInput.Amount}, '{betInput.PlayerName}' );", db);

                    await insertData.ExecuteNonQueryAsync();
                    return Ok("Bet Placed");
                }
                catch(Exception e)
                {
                    // Catch with global class
                    throw new InvalidBetException(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("SpinWheel")]
        public async Task<OkObjectResult> SpinWheel()
        {
            Random random = new Random();
            int selected = random.Next(0,37);
            
            int colorNum = random.Next(0, 2);
            string color = colorNum==1?"red":"black";

            string message = $"Color: {color}, Number: {selected}";
            if (SpinsHistory.History == null)
            {
                SpinsHistory.History = new List<string>();
            }
            SpinsHistory.History.Add(message);

            return Ok(message);
        }

        [HttpGet]
        [Route("Payout")]
        public async Task<OkObjectResult> Payout()
        {
            Random random = new Random();
            int payout = random.Next(0, 100000);

            string message = $"Congrats! you have won ${payout}";

            return Ok(message);
        }

        [HttpGet]
        [Route("ShowPreviousSpins")]
        public async Task<OkObjectResult> ShowPreviousSpins()
        {
            var message = string.Empty;
            if (SpinsHistory.History == null)
            {
                throw new InvalidHistoryException("No spins have been spun. Give it a whirl");
            }
            
            SpinsHistory.History.ForEach(h => { message += h + "\n"; });

            return Ok(message);
        }
    }
}