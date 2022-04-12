namespace MyCarterApp.Modules;

using FluentValidation;
using System.Collections.Concurrent;


public interface IDatabase
{
    int StorePerson(Person person);
}

public class Database : IDatabase
{
    private ConcurrentDictionary<int, Person> store = new();

    public int StorePerson(Person person)
    {
        var l = store.Count;
        _ = store.TryAdd(l + 1, person);
        return l + 1;
    }
}


public record Person(string name);

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.name).NotEmpty();
        RuleFor(x => x.name).MinimumLength(3);
        RuleFor(x => x.name).MaximumLength(10);
    }
}
