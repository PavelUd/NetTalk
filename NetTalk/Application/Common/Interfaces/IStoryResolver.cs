namespace Application.Common.Interfaces;

public interface IStoryResolver
{
    TStory Resolve<TStory>() where TStory : IStory;
}