
A console app to update feature flags file.

Currently supports
- Adding a new flag
- Removing an existing flag
- Enabling a flag
- Disabling a flags

Provides a list of available flags and environments when enabling/disabling flags.

## Usage 

1. Update appSettings.json with the correct paths
   1. **enumPath**: Absolute path to the enum file for flags.
   2. **jsonFilesDirectory**: Absolute path to the folder where all the feature flag jsons are stored.
   3. **ownedBy**: A string value for who owns the flag.
   4. **jsonFilesSearchPattern**: The search string to match against the names of files in jsonFilesDirectory.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.
2. Run run.cmd and follow the prompts
