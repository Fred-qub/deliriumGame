using UnityEditor;
public class ModelImporterPostprocessor : AssetPostprocessor
{
    // This script was used to resolve issues with assets being properly available on every machine...corrected a setting which was not properly set on asset import
    void OnPreprocessModel()
    {
        ModelImporter importer = (ModelImporter)assetImporter;
        importer.isReadable = true; // Enables Read/Write
    }
}
