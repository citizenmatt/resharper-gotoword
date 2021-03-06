using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.TreeModels;
using JetBrains.IDE.TreeBrowser;

#if RESHARPER8
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Features.Common.Occurences;
#elif RESHARPER81
using JetBrains.ReSharper.Feature.Services.Tree;
using JetBrains.ReSharper.Feature.Services.Navigation.Search;
#elif RESHARPER9
using JetBrains.ReSharper.Feature.Services.Tree;
using JetBrains.ReSharper.Feature.Services.Navigation;
#endif

namespace JetBrains.ReSharper.GoToWord
{
  public sealed class GotoWordBrowserDescriptor : OccurenceBrowserDescriptor
  {
    [NotNull] private readonly TreeSectionModel myModel;

    public GotoWordBrowserDescriptor(
      [NotNull] ISolution solution, [NotNull] string pattern,
      [NotNull] List<IOccurence> occurrences,
      [CanBeNull] IProgressIndicator indicator = null)
      : base(solution)
    {
      Title.Value = string.Format("Textual occurrences of '{0}'", pattern);
      DrawElementExtensions = true;
      myModel = new TreeSectionModel();

      using (ReadLockCookie.Create())
      {
        // ReSharper disable once DoNotCallOverridableMethodsInConstructor
        SetResults(occurrences, indicator);
      }
    }

    public override TreeModel Model
    {
      get { return myModel; }
    }

    protected override void SetResults(
      ICollection<IOccurence> items, IProgressIndicator indicator = null, bool mergeItems = true)
    {
      base.SetResults(items, indicator, mergeItems);
      RequestUpdate(UpdateKind.Structure, true);
    }
  }
}