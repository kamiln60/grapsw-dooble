using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using DobbleGameServer.game;
using TestClient.menu.command;
using TestClient.remote;

namespace TestClient.menu {
    public class CommandMenu<T>
    {
        private readonly Dictionary<int, Command<T>> commands;

        private readonly T _remote;

        public CommandMenu(T remote)
        {
            this._remote = remote;
            this.commands = new Dictionary<int, Command<T>>();
        }

        public void execute(int commandKey)
        {
            if (!commands.ContainsKey(commandKey))
            {
                Console.WriteLine("Nie znaleziono takiego polecenia!");
                return;
            }
            commands[commandKey].execute();
        }

        public void displayMenu()
        {
            Console.WriteLine("--------------------------");
            commands.Keys
                .Select(key => new Entry<int, Command<T>>(key, commands[key]))
                .Select(entry =>
                {
                    return entry.Key.ToString() + ". " + entry.Value.getName();
                }).OrderBy(entry => entry)
                .ToList()
                .ForEach(Console.WriteLine);
        }

        public void AddCommands(Dictionary<int, Command<T>> cmd)
        {
            cmd.Keys.Select(key => new Entry<int, Command<T>>(key, cmd[key]))
                .ToList()
                .ForEach(entry => commands.Add(entry.Key, entry.Value));
        }
    }

    public class Entry<K, V>
    {
        public K Key { get; set; }

        public V Value { get; set; }

        public Entry(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}
