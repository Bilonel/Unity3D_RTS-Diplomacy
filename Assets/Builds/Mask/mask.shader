Shader "Custom/mask"
{
    SubShader
    {
        Tags { "Queue" = "Transparent+1" }

        Pass { Blend Zero One }
    }
}