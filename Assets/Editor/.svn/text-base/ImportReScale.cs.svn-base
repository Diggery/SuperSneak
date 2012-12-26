using UnityEngine;

using UnityEditor;

using System;

 

//Sets our settings for all new Models and Textures upon first import

public class CustomImportSettings : AssetPostprocessor 

{

    public const float importScale= 1.0f;

    

    void OnPreprocessModel() 

    {

        ModelImporter importer = assetImporter as ModelImporter;

 

        importer.globalScale  = importScale;
 

    }

}
