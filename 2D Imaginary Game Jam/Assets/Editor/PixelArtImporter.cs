using UnityEditor;
using UnityEngine;

public class PixelArtImporter : AssetPostprocessor
{
    [System.Obsolete]
    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter importer = (TextureImporter)assetImporter;

        if (importer.textureType == TextureImporterType.Sprite &&
            importer.spriteImportMode == SpriteImportMode.Multiple)
        {
            int tileWidth = 16;
            int tileHeight = 16;

            importer.isReadable = true;
            importer.spritePixelsPerUnit = 16;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;

            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteMode = (int)SpriteImportMode.Multiple;
            importer.SetTextureSettings(settings);

            importer.spritesheet = GenerateGridSlices(texture.width, texture.height, tileWidth, tileHeight);
        }
    }

    private SpriteMetaData[] GenerateGridSlices(int texWidth, int texHeight, int cellWidth, int cellHeight)
    {
        int cols = texWidth / cellWidth;
        int rows = texHeight / cellHeight;
        var meta = new SpriteMetaData[cols * rows];

        int index = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Rect rect = new Rect(x * cellWidth, texHeight - (y + 1) * cellHeight, cellWidth, cellHeight);

                // Avoid out-of-bounds rects (just in case texture isn't exact multiple of cell size)
                if (rect.xMax > texWidth || rect.yMax > texHeight || rect.width <= 0 || rect.height <= 0)
                    continue;

                SpriteMetaData smd = new SpriteMetaData();
                smd.alignment = (int)SpriteAlignment.Center;
                smd.border = Vector4.zero;
                smd.name = $"tile_{x}_{y}_{index}"; // Ensure unique name
                smd.rect = rect;

                meta[index++] = smd;
            }
        }

        // Trim array in case of skipped tiles
        System.Array.Resize(ref meta, index);
        return meta;
    }
}
