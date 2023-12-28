using osu.Framework.Testing;

namespace KartCityStudio.Game.Tests.Visual
{
    public partial class KartCityStudioTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new KartCityStudioTestSceneTestRunner();

        private partial class KartCityStudioTestSceneTestRunner : KartCityStudioGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
