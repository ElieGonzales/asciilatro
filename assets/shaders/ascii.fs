#if defined(VERTEX) || __VERSION__ > 100 || defined(GL_FRAGMENT_PRECISION_HIGH)
    #define PRECISION highp
#else
    #define PRECISION mediump
#endif


extern PRECISION vec2 ascii;
extern PRECISION float time;
extern PRECISION vec2 char_size = vec2(8.0, 8.0);
extern PRECISION float character_amount = 10.0;
extern PRECISION Image character_texture;
extern PRECISION int w;
extern PRECISION int h;



// This is what actually changes the look of card
vec4 effect( vec4 colour, Image texture, vec2 texture_coords, vec2 screen_coords )
{
    //SCREEN_COORDS is a vec2 of the pixel's position on the screen, in pixels
    //TEXTURE_COORDS is the UV coordinates of the pixel, in the range [0, 1];
    vec2 screen_size = vec2(w, h);

    vec4 tex = Texel(texture, texture_coords);

    //Block sampling
    vec2 block_coords = floor(screen_coords / char_size) * char_size;
    vec2 sample_uv = (block_coords + char_size * 0.5) / screen_size;
    vec4 block_color = Texel(texture, sample_uv);

    // Luminance -> character index
    float luminance = 0.2126*block_color.r + 0.7152*block_color.g + 0.0722*block_color.b;
    float character_index = clamp(floor(luminance * (character_amount - 1.0) + 0.5), 0.0, character_amount - 1.0);


    // Sampling character texture
    vec2 incharacter_pos = mod(screen_coords, char_size);
    ivec2 target_pixel_coords = ivec2(character_index * char_size.x + incharacter_pos.x, incharacter_pos.y);

    vec4 character_out = Texel(character_texture, vec2(target_pixel_coords) / vec2(char_size.x * character_amount, char_size.y));

    // Normalize colors to avoid darkening
    float max_color = max(block_color.r, max(block_color.g, block_color.b));
    colour.rgb /= (max_color > 0.0) ? max_color : 1.0;
    block_color.rgb /= (max_color > 0.0) ? max_color : 1.0;

	tex = block_color * character_out;
    return tex;
}