using System.Text.RegularExpressions;

namespace StreamingSolutionsUSA.Bluesky.Validators;

public class BskyDidValidator
{
	public void EnsureValidDid(string did)
	{
		if (!BskyDidRegexes.DidRegex().IsMatch(did))
		{
			throw new InvalidBskyDidException($"Disallowed characters in DID: {did}. Only alphanumeric characters, colons, underscores, percent signs, and hyphens are allowed.");
		}
		string[] parts = did.Split(':');
		if (parts.Length < 3)
		{
			throw new InvalidBskyDidException("DID requires prefix, method, and method-specific content.");
		}

		if (parts[0] != "did")
		{
			throw new InvalidBskyDidException("DID must start with 'did:' prefix.");
		}

		if (!BskyDidRegexes.DidSecondPartRegex().IsMatch(parts[1]))
		{
			throw new InvalidBskyDidException("Invalid method in DID, only lowercase letters are allowed.");
		}

		if (did.EndsWith(':') || did.EndsWith('%'))
		{
			throw new InvalidBskyDidException("DID can not end with \":\" or \"%\".");
		}

		if (did.Length > 8 * 1024)
		{
			throw new InvalidBskyDidException("DID is far too long.");
		}
	}
}

public static partial class BskyDidRegexes
{
	[GeneratedRegex("^[a-zA-Z0-9._:%-]*$")]
	public static partial Regex DidRegex();
	
	[GeneratedRegex("^[a-z]+$")]
	public static partial Regex DidSecondPartRegex();
}

public class InvalidBskyDidException(string message) : ApplicationException(message);
