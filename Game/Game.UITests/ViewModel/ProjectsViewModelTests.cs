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

    }

    public class ProjectDataProviderMock : IProjectDataProvider
    {
        public IEnumerable<IProject> GetAllProjects()
        {
            yield return new Project(new DateTime(2018,11,5));
            yield return new Project(new DateTime(2017,10,4));
            yield return new Project(new DateTime(2016,9,3));
        }
    }

}
