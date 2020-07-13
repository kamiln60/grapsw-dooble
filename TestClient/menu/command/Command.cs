﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.remote;

namespace TestClient.menu.command {
    public abstract class Command<R> {
        protected string _name;

        protected R remote;

        public Command(R remote, string name) {
            _name = name;
            this.remote = remote;
        }

        public string getName() {
            return this._name;
        }

        public abstract void execute();
    }

    public class JoinCommand : Command<Server> {
        public JoinCommand(Server remote, string name) : base(remote, name) {
        }
        public override void execute() {
            Console.WriteLine("Wpisz nazwę: ");
            remote.Connect(Console.ReadLine());
        }
    }

    public class DeclareReadinessCommand : Command<Server>
    {
        public DeclareReadinessCommand(Server remote, string name) : base(remote, name)
        {
        }
        public override void execute()
        {
            remote.Ready = !remote.Ready;
            remote.DeclareReadiness(remote.Token, remote.Ready);
            Console.WriteLine(remote.Ready?"Zaznaczono gotowość":"Odznaczono gotowość");
        }
    }
}
