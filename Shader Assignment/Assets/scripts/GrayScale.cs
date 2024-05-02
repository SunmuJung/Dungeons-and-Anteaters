using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GrayscaleRenderer), PostProcessEvent.AfterStack, "Custom/Grayscale")]
public sealed class Grayscale : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Total effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

     [Range(0f, 1f), Tooltip("Red effect intensity.")]
    public FloatParameter red = new FloatParameter { value = 0.5f };

     [Range(0f, 1f), Tooltip("Green effect intensity.")]
    public FloatParameter green = new FloatParameter { value = 0.5f };

     [Range(0f, 1f), Tooltip("Blue effect intensity.")]
    public FloatParameter blue = new FloatParameter { value = 0.5f };
}

public sealed class GrayscaleRenderer : PostProcessEffectRenderer<Grayscale>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Grayscale"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_red", settings.red);
        sheet.properties.SetFloat("_green", settings.green);
        sheet.properties.SetFloat("_blue", settings.blue);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
