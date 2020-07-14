using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobbleGameServer {
    public partial class DobbleServer {

        //private ConcurrentDictionary<int, Symbol> Symbols;

        //private readonly string CARDS_PATH = "C:\\cards";

        //dla konstruktora: 
        //this.Symbols = new ConcurrentDictionary<int, Symbol>();
        // if (!Directory.Exists(CARDS_PATH))
        // {
        //     
        //     Directory.CreateDirectory(CARDS_PATH);
        //     Console.WriteLine("Utworzono katalog dla kart.");
        // }


        // private bool IsImage(string name)
        // {
        //     return name.EndsWith(".png") || name.EndsWith(".jpg") || name.EndsWith(".bmp");
        // }
        // private void LoadSymbolsFromFiles()
        // {
        //     var filesToLoad = Directory
        //         .GetFiles(CARDS_PATH)
        //         .Where(IsImage)
        //         .ToArray();
        //
        //     int i = 0;
        //     foreach (var file in filesToLoad)
        //     {
        //         i++;
        //         byte[] imageBytes = File.ReadAllBytes(file);
        //         this.Symbols.TryAdd(i, new Symbol(i, imageBytes));
        //     }
        // }
        // Stara wersja - do przywrócenia jak ogarnięte zostanie przesyłanie obrazków przez sieć
        // private void GenerateCardsFromSchema()
        // {
        //     int cardNo = 1;
        //     foreach (var card in CardSchema)
        //     {
        //         Card cardToAdd = new Card();
        //         cardToAdd.Id = cardNo++;
        //         cardToAdd.Symbols = new Dictionary<int, Symbol>();
        //         foreach (var symbolId in card)
        //         {
        //             Symbol symbolToAdd;
        //             Symbols.TryGetValue(symbolId, out symbolToAdd);
        //             cardToAdd.Symbols.Add(symbolId, symbolToAdd);
        //         }
        //
        //         this.Cards.TryAdd(cardToAdd.Id, cardToAdd);
        //     }
        // }
    }
}
