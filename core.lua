SMODS.Atlas { key = "ascii_tex", path = "ascii.png", px = 80, py = 8 }
Ascii_tex = love.graphics.newImage("Mods/ASCIIlatro/assets/ascii.png")

SMODS.ScreenShader { key = "ascii", path = "ascii.fs", 
    should_apply = function(self)
        return true
    end,
    send_vars = function(self)
        local w,h = love.graphics.getDimensions()
        return {
            --char_size = {array = {8.0, 8.0}},
            --character_amount = 10.0,
            character_texture = Ascii_tex,       
            w = w,
            h = h
        }
    end
}