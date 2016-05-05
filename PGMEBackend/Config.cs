// Awesome Map Editor
// A map editor for GBA Pokémon games.

// Copyright (C) 2015 Diegoisawesome

// This file is part of Awesome Map Editor.
// Awesome Map Editor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Awesome Map Editor is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Awesome Map Editor. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using static PGMEBackend.Program;

namespace PGMEBackend
{
    static class Config
    {
        public static GameList gameList;
        public static CharTable charTable;

        static string gamesFile = AppDomain.CurrentDomain.BaseDirectory + @"Games.yaml";
        static string tableFile = AppDomain.CurrentDomain.BaseDirectory + @"Table.yaml";

        public static int ReadConfig()
        {
            var deserializer = new Deserializer();

            using (StreamReader input = new StreamReader(gamesFile))
            {
                gameList = deserializer.Deserialize<GameList>(input);
            }

            using (StreamReader input = new StreamReader(tableFile))
            {
                charTable = deserializer.Deserialize<CharTable>(input);
            }

            return 0;
        }

        public class GameList
        {
            public Dictionary<string, Game> Games { get; set; }
        }

        public class CharTable
        {
            public Dictionary<int, string> Table { get; set; }
        }

        public class Game
        {
            public string RomCode { get; set; }
            public string RomType { get; set; }
            public string Name { get; set; }
            public string Language { get; set; }

            public int MapBanks { get; set; }
            public int MapLayouts { get; set; }
            public short MapLayoutCount { get; set; }
            public int MapNames { get; set; }
            public int MapNameStart;
            public int MapNameCount { get; set; }
            public int MapNameTotal { get; set; }
            public byte MapBankCount { get; set; }
            public byte[] MapBankSizes { get; set; }
            
            public int MainTSPalCount { get; set; }
            public int MainTSBlocks { get; set; }
            public int LocalTSBlocks { get; set; }
            public short MainTSSize { get; set; }
            public short LocalTSSize { get; set; }

            public byte WorldMapCount { get; set; }
            public int[] WorldMapGFX { get; set; }
            public int[] WorldMapTileMap { get; set; }
            public int[] WorldMapPal { get; set; }

            public int FreespaceStart { get; set; }
            public byte FreespaceByte { get; set; }
            
            public IDictionary<int, string> Songs;

            public Game DereferencePointers()
            {
                var game = (Game)MemberwiseClone();
                game.MapBanks = ROM.ReadPointer(game.MapBanks);
                game.MapLayouts = ROM.ReadPointer(game.MapLayouts);
                game.MapNames = ROM.ReadPointer(game.MapNames);
                game.MapNameTotal = ROM.GetData(game.MapNameTotal, 1)[0];
                if (game.RomType == "FRLG")
                    game.MapNameTotal--;
                if (game.MapNameCount != 0)
                    game.MapNameCount = ROM.GetData(game.MapNameCount, 1)[0];
                else
                    game.MapNameCount = game.MapNameTotal;
                game.MapNameStart = (byte)(game.MapNameTotal - game.MapNameCount);
                return game;
            }
        }
    }
}
