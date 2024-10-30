using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using VoodooDoodoo.Puzzle;

namespace VoodooDoodoo.Utils.Editor
{
    public sealed class LevelsDatabase : OdinMenuEditorWindow
    {
        [MenuItem("VoodooDoodoo/Levels Database")]
        private static void OpenWindow()
        {
            LevelsDatabase window = GetWindow<LevelsDatabase>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
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
            tree.AddAllAssetsAtPath("Levels", "Assets/Scripts/Levels", typeof(PuzzleLevelData), true);

            tree.EnumerateTree()
                .AddThumbnailIcons()
                .SortMenuItemsByName();

            return tree;
        }
    }
}
