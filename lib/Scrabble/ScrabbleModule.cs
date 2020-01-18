using Autofac;
using Mijabr.Language;
using Scrabble.Ai;
using Scrabble.Draw;
using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Play;
using Scrabble.Value;
using System.IO.Abstractions;
using words;

namespace Scrabble
{
    public class ScrabbleModule : Module
    {
        //public static IFileSystem FileSystem { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ItemLister>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<IsWordRequestHandler>().InstancePerLifetimeScope();
            builder.RegisterType<FindWordsRequestHandler>().InstancePerLifetimeScope();
            builder.RegisterType<ScrabbleManager>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Board>().InstancePerLifetimeScope();
            builder.RegisterType<TileDrawer>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GoHandler>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GoValidator>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GridModel>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GoWordFinder>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GoWordValidator>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GoMessageMaker>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GoScorer>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AiGoHandler>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AiGridModel>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AiGoWordFinder>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AiGoPlacer>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<GameRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GameFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DateTimeOffsetClass>().AsImplementedInterfaces().SingleInstance();

            RegisterScrabbleDictionary(builder);
        }

        private static void RegisterScrabbleDictionary(ContainerBuilder builder)
        {
//            var wordDictionary = new WordDictionary(FileSystem);
//#if DEBUG
//            wordDictionary.LoadFile(@"..\..\Scrabble\words\dictionary.txt");
//#else
//            wordDictionary.LoadFile("dictionary.txt");
//#endif
//            builder.Register<WordValidatable>((c) => wordDictionary);
//            builder.Register<WordFindable>((c) => wordDictionary);

            builder.RegisterType<WordDictionary>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FileSystem>().AsImplementedInterfaces();
        }
    }
}
