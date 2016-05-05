namespace CustomBA
{
    /// <summary>
    ///     This will track which phase of the bootstrapping process we are in so that we can enable and disable buttons as
    ///     appropriate.
    ///     It will also allow us to track whether the user has canceled the install.
    ///     If they have, and we're already installing—otherwise known as Applying—we will know to not immediately shut down
    ///     the process,
    ///     but rather send a flag to the bootstrapper so that it can roll back any installed packages.
    ///     If, on the other hand, we have not begun the Apply phase, it is safe to shut down the bootstrapper immediately.
    /// </summary>
    public enum InstallState
    {
        Initializing,
        Present,
        NotPresent,
        Applying,
        Cancelled
    }
}