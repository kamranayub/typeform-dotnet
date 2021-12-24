// DO NOT USE
//
// We ultimately might need to fork and fix semantic-release-nuget
// package to properly pass the /p:Version value by default
// from the Commit Analyzer Semantic Release plugin.
//
module.exports = {
  verifyConditions: (pluginConfig, { logger, nextRelease: { version } }) => {
    logger.info("Setting NUGET_VERSION env variable", version);
    process.env.NUGET_VERSION = version;
  },
};
