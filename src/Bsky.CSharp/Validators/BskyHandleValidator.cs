using System.Text.RegularExpressions;
using StreamingSolutionsUSA.Bluesky.Exceptions;

namespace StreamingSolutionsUSA.Bluesky.Validators;

public class BskyHandleValidator
{
	/// <summary>
	/// Validates a Bluesky handle to ensure it adheres to specific format and character rules.
	/// </summary>
	/// <param name="handle">The handle string to be validated.</param>
	/// <exception cref="InvalidBskyHandleException">
	/// Thrown when the handle contains disallowed characters, exceeds maximum length,
	/// has an insufficient number of domain parts, contains empty segments,
	/// has overly long segments, starts or ends with a hyphen, or has an invalid top-level domain.
	/// </exception>
	public void ValidateHandle(string handle)
	{
		if (!BskyHandleRegex.ValidHandle().IsMatch(handle))
		{
			throw new InvalidBskyHandleException("Disallowed characters in handle. Only alphanumeric characters, dots, and dashes are allowed.");
		}

		if (handle.Length > 253)
		{
			throw new InvalidBskyHandleException("Handle exceeds maximum length of 253 characters.");
		}
		
		string[] labels = handle.Split('.');
		if (labels.Length < 2)
		{
			throw new InvalidBskyHandleException("Handle domain needs at least 2 parts");
		}

		for (int i = 0; i < labels.Length; i++)
		{
			string label = labels[i];
			if (string.IsNullOrWhiteSpace(label))
			{
				throw new InvalidBskyHandleException("Handle parts cannot be empty");
			}

			if (label.Length > 63)
			{
				throw new InvalidBskyHandleException("Handle part too long (max 63 characters)");
			}

			if (label.EndsWith('-') || label.StartsWith('-'))
			{
				throw new InvalidBskyHandleException("Handle parts cannot start or end with a hyphen");
			}

			if (i + 1 == labels.Length && !BskyHandleRegex.ValidHandlePart().IsMatch(label))
			{
				throw new InvalidBskyHandleException("Handle final component (TLD) must start with ASCII letter");
			}
		}
	}

	public string NormalizeHandle(string handle) => handle.ToLower();

	public string NormalizeAndEnsureValidHandle(string handle)
	{
		string normalizedHandle = NormalizeHandle(handle);
		ValidateHandle(handle);
		return normalizedHandle;
	}
	
	public bool IsValidHandle(string handle)
	{
		try
		{
			ValidateHandle(handle);
			return true;
		}
		catch (InvalidBskyHandleException)
		{
			return false;
		}
	}
}

public static partial class BskyHandleRegex
{
	[GeneratedRegex("^[a-zA-Z0-9.-]*$")]
	public static partial Regex ValidHandle();

	[GeneratedRegex("^[a-zA-Z]")]
	public static partial Regex ValidHandlePart();
}

