module.exports = {
  verifyConditions: (pluginConfig, { logger, nextRelease: { version } }) => {
    logger.info("Setting NUGET_VERSION env variable", version);
    process.env.NUGET_VERSION = version;
  },
};
