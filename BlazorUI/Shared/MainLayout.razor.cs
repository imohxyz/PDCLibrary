namespace Cinema9.BlazorUI.Shared;

public partial class MainLayout
{
    private readonly MudTheme myTheme = new()
    {
        PaletteLight = new PaletteLight
        {
            AppbarBackground = "#1c2780"
        },
        Typography = new Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = ["Rubik", "sans-serif"],
                FontSize= "0.813rem"
            },
            H1 = new H1Typography()
            {
                FontSize = "1.875rem"
            },
            H2 = new H2Typography()
            {
                FontSize = "1.375rem"
            },
            H3 = new H3Typography()
            {
                FontSize = "1.25rem"
            },
            H4 = new H4Typography()
            {
                FontSize = "1.125rem"
            },
            H5 = new H5Typography()
            {
                FontSize = "1rem"
            },
            H6 = new H6Typography()
            {
                FontSize = "0.875rem"
            }
        }
    };

    bool _drawerOpen = true;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}