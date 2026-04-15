using UnityEditor;
public class ModelImporterPostprocessor : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter importer = (ModelImporter)assetImporter;
        importer.isReadable = true; // Enables Read/Write
    }
}
