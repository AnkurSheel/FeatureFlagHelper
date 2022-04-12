
A console app to update feature flags file.

Currently supports
- Adding a new flag
- Removing an existing flag
- Enabling a flag
- Disabling a flags

Provides a list of available flags and environments when enabling/disabling flags.

## Usage 

1. Update appSettings.json with the correct paths
   1. **enumPath**: Absolute path to the num file for flags
   2. **jsonFilesDirectory**: Absolute path to the folder where all the feature flag jsons are stored
2. Run run.cmd and follow the prompts
