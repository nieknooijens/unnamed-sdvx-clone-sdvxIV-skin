#version 330
#extension GL_ARB_separate_shader_objects : enable

layout(location=1) in vec2 fsTex;
layout(location=0) out vec4 target;

uniform sampler2D mainTex;
uniform vec4 lCol;
uniform vec4 rCol;
uniform float hidden;

void main()
{	
	vec4 mainColor = texture(mainTex, vec2(fsTex.x, 0.75));
	vec4 replaceColor = texture(mainTex, vec2(fsTex.x, 0.25));
	
    vec4 col = mainColor;

    if(fsTex.y > hidden * 0.6)
    {
		vec4 overlay = vec4(0, 0, 0, replaceColor.w);
		
        //Red channel to color right lane
        overlay.xyz += vec3(.9) * rCol.xyz * vec3(replaceColor.x);

        //Blue channel to color left lane
        overlay.xyz += vec3(.9) * lCol.xyz * vec3(replaceColor.z);

        //Color green channel white
        overlay.xyz += vec3(.6) * vec3(replaceColor.y);
		
		float alpha = max(col.w, overlay.w);
		col = vec4(col.xyz * (1 - overlay.w) + overlay.xyz * overlay.w, alpha);
    }
    else
    {
        col.xyz = vec3(0.);
        col.a = col.a > 0.0 ? 0.3 : 0.0;
    }
	
    target = col;
}