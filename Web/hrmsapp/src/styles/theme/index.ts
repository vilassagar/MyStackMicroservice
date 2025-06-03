// src/styles/theme/index.ts
import { colors } from './colors';
import { spacing } from './spacing';
import { typography } from './typography';
import { shadows } from './shadows';

export const theme = {
    colors,
    spacing,
    typography,
    shadows,
    // Other theme variables can be added here
};

export type ThemeColors = typeof colors.light;
export type ThemeColorName = keyof typeof colors;

export default theme;