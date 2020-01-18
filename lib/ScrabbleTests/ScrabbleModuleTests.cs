using System;
using System.Reflection;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mijabr.Language;
using Scrabble;
using Scrabble.Ai;
using Scrabble.Draw;
using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Play;
using Scrabble.Value;
using Shouldly;
using words;

namespace ScrabbleTests
{
    [TestClass]
    public class ScrabbleModuleTests
    {
        private ContainerBuilder builder;
        private ScrabbleModule module;
        private IContainer container;
        private ILifetimeScope scope1;
        private ILifetimeScope scope2;

        [TestInitialize]
        public void SetUp()
        {
            builder = new ContainerBuilder();
            module = new ScrabbleModule();
        }

        private void WhenILoadModule()
        {
            typeof(ScrabbleModule).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(module, new object[] { builder });
            container = builder.Build();
            scope1 = container.BeginLifetimeScope();
            scope2 = container.BeginLifetimeScope();
        }

        private void AssertScopedServicesHaveSameInstance(Type type)
        {
            scope1.Resolve(type).ShouldBe(scope2.Resolve(type));
        }

        [TestMethod]
        public void TestRegistrations()
        {
            WhenILoadModule();
            container.Resolve(typeof(IItemLister)).ShouldNotBeNull();

            container.Resolve(typeof(IDateTimeOffset)).ShouldNotBeNull();
            container.Resolve(typeof(WordValidatable)).ShouldNotBeNull();
            container.Resolve(typeof(WordFindable)).ShouldNotBeNull();
            container.Resolve(typeof(IsWordRequestHandler)).ShouldNotBeNull();
            container.Resolve(typeof(FindWordsRequestHandler)).ShouldNotBeNull();
            container.Resolve(typeof(IGoWordValidator)).ShouldNotBeNull();
            container.Resolve(typeof(IScrabbleManager)).ShouldNotBeNull();
            container.Resolve(typeof(Board)).ShouldNotBeNull();
            container.Resolve(typeof(ITileDrawer)).ShouldNotBeNull();
            container.Resolve(typeof(IGoHandler)).ShouldNotBeNull();
            container.Resolve(typeof(IGoValidator)).ShouldNotBeNull();
            container.Resolve(typeof(IGridModel)).ShouldNotBeNull();
            container.Resolve(typeof(IGoWordFinder)).ShouldNotBeNull();
            container.Resolve(typeof(IGoMessageMaker)).ShouldNotBeNull();
            container.Resolve(typeof(IAiGoWordFinder)).ShouldNotBeNull();
            container.Resolve(typeof(IAiGoPlacer)).ShouldNotBeNull();
            container.Resolve(typeof(IAiGoHandler)).ShouldNotBeNull();
            container.Resolve(typeof(IGameRepository)).ShouldNotBeNull();
            container.Resolve(typeof(IGameFactory)).ShouldNotBeNull();
        }

        [TestMethod]
        public void RegistrationShouldBeSingleInstance()
        {
            WhenILoadModule();
            AssertScopedServicesHaveSameInstance(typeof(IItemLister));
            AssertScopedServicesHaveSameInstance(typeof(IGameRepository));
            AssertScopedServicesHaveSameInstance(typeof(IGameFactory));
            AssertScopedServicesHaveSameInstance(typeof(IDateTimeOffset));
        }
    }
}
