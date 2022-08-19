using UnityEditor;
using UnityEngine;

public class ftSettingsProvider
{
    static BakeryProjectSettings pstorage;

    static void GUIHandler(string searchContext)
    {
        if (pstorage == null) pstorage = ftLightmaps.GetProjectSettings();
        if (pstorage == null) return;

        var so = new SerializedObject(pstorage);

        var prev = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 280;

        EditorGUILayout.PropertyField(so.FindProperty("mipmapLightmaps"), new GUIContent("Mipmap Lightmaps", "Enable mipmapping on lightmap assets. Can cause leaks across UV charts as atlases get smaller."));
        EditorGUILayout.PropertyField(so.FindProperty("format8bit"), new GUIContent("Mask/Direction format", ""));
        EditorGUILayout.PropertyField(so.FindProperty("texelPaddingForDefaultAtlasPacker"), new GUIContent("Texel padding (Default atlas packer)", "How many empty texels to add between objects' UV layouts in lightmap atlases."), GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(so.FindProperty("texelPaddingForXatlasAtlasPacker"), new GUIContent("Texel padding (xatlas packer)", "How many empty texels to add between objects' UV layouts in lightmap atlases."));
        EditorGUILayout.PropertyField(so.FindProperty("alphaMetaPassResolutionMultiplier"), new GUIContent("Alpha Meta Pass resolution multiplier", "Scales resolution for alpha Meta Pass maps."));
        //EditorGUILayout.PropertyField(so.FindProperty("volumeRenderMode"), new GUIContent("Volume render mode", "Render mode for volumes."));

        var volMode = (BakeryLightmapGroup.RenderMode)so.FindProperty("volumeRenderMode").intValue;
        var newVolMode = (BakeryLightmapGroup.RenderMode)EditorGUILayout.EnumPopup(new GUIContent("Volume render mode", "Render mode for volumes."), volMode);
        if (volMode != newVolMode) so.FindProperty("volumeRenderMode").intValue = (int)newVolMode;

        EditorGUILayout.PropertyField(so.FindProperty("deletePreviousLightmapsBeforeBake"), new GUIContent("Delete previous lightmaps before bake", "Should previously rendered Bakery lightmaps be deleted before the new bake?"));
        EditorGUILayout.PropertyField(so.FindProperty("logLevel"), new GUIContent("Log level", "Print information about the bake process to console? 0 = don't. 1 = info only; 2 = warnings only; 3 = everything."));
        EditorGUILayout.PropertyField(so.FindProperty("alternativeScaleInLightmap"), new GUIContent("Alternative Scale in Lightmap", "Make 'Scale in Lightmap' renderer property act more similar to built-in Unity behaviour."));
        EditorGUILayout.PropertyField(so.FindProperty("generateSmoothPos"), new GUIContent("Generate smooth positions", "Should we adjust sample positions to prevent incorrect shadowing on very low-poly meshes with smooth normals?"));

        EditorGUIUtility.labelWidth = prev;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Revert to defaults"))
        {
            if (EditorUtility.DisplayDialog("Bakery", "Revert Bskery project settings to default?", "Yes", "No"))
            {
                so.FindProperty("mipmapLightmaps").boolValue = false;
                so.FindProperty("format8bit").intValue = 0;
                so.FindProperty("texelPaddingForDefaultAtlasPacker").intValue = 3;
                so.FindProperty("texelPaddingForXatlasAtlasPacker").intValue = 1;
                so.FindProperty("alphaMetaPassResolutionMultiplier").intValue = 2;
                so.FindProperty("volumeRenderMode").intValue = 1000;
                so.FindProperty("deletePreviousLightmapsBeforeBake").boolValue = false;
                so.FindProperty("logLevel").intValue = 3;
                so.FindProperty("alternativeScaleInLightmap").boolValue = false;
                so.FindProperty("generateSmoothPos").boolValue = true;
            }
        }

        so.ApplyModifiedPropertiesWithoutUndo();
    }

#if UNITY_2018_3_OR_NEWER
    [SettingsProvider]
    public static SettingsProvider CreateSettingsProvider()
    {
        var provider = new SettingsProvider("Project/BakeryGlobalSettings", SettingsScope.Project);
        provider.label = "Bakery GPU Lightmapper";
        provider.guiHandler = GUIHandler;
        return provider;
    }
#endif
}
