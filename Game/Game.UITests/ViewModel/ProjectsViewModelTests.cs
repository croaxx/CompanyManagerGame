using Game.UI.ViewModel;
using Xunit;
using FluentAssertions;
using Game.UI.DataProvider;
using Game.Model;
using System;
using System.Collections.Generic;

namespace Game.UITests.ViewModel
{
    public class ProjectsViewModelTests
    {
        [Fact]
        public void ShouldLoadProjects()
        {
            var viewModel = new ProjectsViewModel(new ProjectDataProviderMock());

            viewModel.Load();

            viewModel.Projects.Count.Should().Be(3);

            viewModel.Projects[0].Expiry.Should().Be(new DateTime(2018,11,5));
        }

    }

    public class ProjectDataProviderMock : IProjectDataProvider
    {
        public IEnumerable<Project> GetAllProjects()
        {
            yield return new Project(new DateTime(2018,11,5));
            yield return new Project(new DateTime(2017,10,4));
            yield return new Project(new DateTime(2016,9,3));
        }
    }

}
