using AutoFixture;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuctionApp.Tests;
public class TimeSpanGenerator : ISpecimenBuilder
{
    private readonly RandomNumericSequenceGenerator randomizer;

    public TimeSpanGenerator()
    : this(TimeSpan.FromMinutes(10), TimeSpan.FromHours(5))
    {
    }

    public TimeSpanGenerator(TimeSpan minSpan, TimeSpan maxSpan)
    {
        if (minSpan >= maxSpan)
        {
            throw new ArgumentException("The 'minSpan' argument must be less than the 'maxSpan'.");
        }

        randomizer = new RandomNumericSequenceGenerator(minSpan.Ticks, maxSpan.Ticks);
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        if (!IsNotDateTimeRequest(request))
        {
            return CreateRandomDate(context);
        }

        return new NoSpecimen();
    }

    private static bool IsNotDateTimeRequest(object request)
    {
        return !typeof(TimeSpan).GetTypeInfo().IsAssignableFrom(request as Type);
    }

    private object CreateRandomDate(ISpecimenContext context)
    {
        return new TimeSpan(GetRandomNumberOfTicks(context));
    }

    private long GetRandomNumberOfTicks(ISpecimenContext context)
    {
        return (long)randomizer.Create(typeof(long), context);
    }
}