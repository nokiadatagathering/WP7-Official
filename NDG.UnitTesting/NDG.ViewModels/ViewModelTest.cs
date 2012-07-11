using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDG.ViewModels;

namespace NDG.UnitTesting.NDG.ViewModels
{
    [TestClass]
    public class ViewModelTest : SilverlightTest
    {
        [TestMethod]
        public void CreationTest()
        {
            ViewModel viewModel = new ViewModel();
            viewModel.BusyCount++;
            Assert.IsTrue(viewModel.IsBusy);
            viewModel.BusyCount--;
            Assert.IsFalse(viewModel.IsBusy);
        }

    }
}
