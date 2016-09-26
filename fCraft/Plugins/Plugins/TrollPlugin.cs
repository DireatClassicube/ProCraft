﻿using fCraft;

namespace TrollPlugin {

    public class Init : Plugin {

        public void Initialize() {
            Logger.Log( LogType.ConsoleOutput, Name + "(v " + Version + "): Registering TrollPlugin" );
            CommandManager.RegisterCustomCommand( CdTroll );
        }

        private static CommandDescriptor CdTroll = new CommandDescriptor {
            Name = "Troll",
            Category = CommandCategory.Chat | CommandCategory.New,
            Permissions = new[] { Permission.ReadStaffChat },
            IsConsoleSafe = true,
            NotRepeatable = false,
            Usage = "/troll player option [Message]",
            Help = "Does a little somthin'-somethin'.&n" +
            " Available options: st, ac, pm, message or leave",
            Handler = TrollHandler
        };

        //your plugin name
        public string Name { get { return "TrollPlugin"; } set { } }

        //your plugin version
        public string Version { get { return "1.0"; } set { } }

        private static void TrollHandler( Player player, CommandReader cmd ) {
            string Name = cmd.Next();
            if ( Name == null ) {
                player.Message( "Player not found. Please specify valid name." );
                return;
            }
            if ( !Player.IsValidPlayerName( Name ) )
                return;
            Player target = Server.FindPlayerOrPrintMatches( player, Name, SearchOptions.Default );
            if ( target == null )
                return;
            string options = cmd.Next();
            if ( options == null ) {
                CdTroll.PrintUsage( player );
                return;
            }
            string Message = cmd.NextAll();
            if ( Message.Length < 1 && !options.CaselessEquals("leave")) {
                player.Message( "&WError: Please enter a message for {0}.", target.ClassyName );
                return;
            }
            switch ( options.ToLower() ) {
                case "pm":
                    if ( player.Can( Permission.UseColorCodes ) && Message.Contains( "%" ) ) {
                        Message = Chat.ReplacePercentColorCodes( Message, false );
                    }
                    Server.Players.Message( "&Pfrom {0}: {1}",
                        target.Name, Message );
                    break;

                case "st":
                case "staff":
                    Chat.SendStaff( target, Message );
                    break;

                case "i":
                case "impersonate":
                case "msg":
                case "message":
                case "m":
                    Server.Message( "{0}&S&F: {1}",
                                      target.ClassyName, Message );
                    break;

                case "leave":
                case "disconnect":
                case "gtfo":
                    Server.Players.Message( "&SPlayer {0}&S left the server.",
                        target.ClassyName );
                    break;

                default:
                    player.Message( "Invalid option. Please choose st, ac, pm, message or leave" );
                    break;
            }
        }
    }
}