using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using VoodooDoodoo.Puzzle;

namespace VoodooDoodoo.Utils.Editor
{
    public class ElementsDatabase : OdinMenuEditorWindow
    {
        [MenuItem("VoodooDoodoo/Puzzle Elements Database")]
        private static void OpenWindow()
        {
            ElementsDatabase window = GetWindow<ElementsDatabase>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 500);
        }

        protected override OdinMenuTree BuildMenuTree ()
        {
            OdinMenuTree tree = new(true);

            OdinMenuStyle customMenuStyle = new()
            {
                BorderPadding = 0f,
                AlignTriangleLeft = true,
                TriangleSize = 16f,
                TrianglePadding = 0f,
                Offset = 20f,
                Height = 23,
                IconPadding = 0f,
                BorderAlpha = 0.323f
            };

            tree.DefaultMenuStyle = customMenuStyle;
            tree.Config.DrawSearchToolbar = true;
            tree.AddAllAssetsAtPath("Elements", "Assets/Scripts/Puzzle/PuzzleElements", typeof(PuzzleElementData), true);

            tree.EnumerateTree()
                .AddThumbnailIcons()
                .SortMenuItemsByName();

            return tree;
        }
    }
}
