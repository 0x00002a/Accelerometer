# Accelerometer Changelog

## 1.1.0

Update to the UI and some fixes 

### Features

- Now has config options avaliable through the TextHudAPI menu. See the description for what each of them do 
- Tested in multiplayer on hosted and dedicated 
- Can display in `g`'s or `m/s^-2`
- Uses a proper superscript 2 rather than `^2` 
- Shows leading 0s so it no longer jumps about when updating 

### Fixes

- Not working in dedicated multiplayer 
- Memory leak and multiple drawing (didn't dispose of the TextApi handle properly)

## 1.0.0

Initial release
