using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Stories;

public class StoryResolver(IServiceProvider serviceProvider) : IStoryResolver
{
    public TStory Resolve<TStory>() where TStory : IStory
    {
        return serviceProvider.GetRequiredService<TStory>();
    }
}