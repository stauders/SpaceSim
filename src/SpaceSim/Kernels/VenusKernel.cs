﻿using SpaceSim.SolarSystem;

namespace SpaceSim.Kernels
{
    class VenusKernel : BaseKernel, IMassiveKernel
    {
        // -------- ------ Image ----- --------
        // 01111111 00000000 00000000 00000000 alpha >> 24
        // 00000000 11111111 00000000 00000000 red >> 16
        // 00000000 00000000 11111111 00000000 green >> 8
        // 00000000 00000000 00000000 11111111 blue
        public void Run(int[] image, int resX, int resY, double cX, double cY, double cWidth, double cHeight, double cRot, double sunNormalX, double sunNormalY, double bodyX, double bodyY, double bodyRot)
        {
            int index = get_global_id(0);

            double diffX, diffY;
            ComputeCameraRotation(index, resX, resY, cX, cY, cWidth, cHeight, cRot, bodyX, bodyY, out diffX, out diffY);

            double distance = sqrt(diffX * diffX + diffY * diffY);

            if (distance < VENUS_RADIUS + VENUS_ATMOSPHERE)
            {
                double worldNormalX, worldNormalY, sunDotProduct;
                ComputeSunShading(diffX, diffY, distance, sunNormalX, sunNormalY, out worldNormalX, out worldNormalY, out sunDotProduct);

                if (distance < VENUS_RADIUS)
                {
                    double ratio = distance / VENUS_RADIUS;

                    double worldAngle = atan2(worldNormalX, worldNormalY) + atan2(ratio, 1.1) + bodyRot;

                    double textureFactor = 0.25 * sin(worldAngle * 85) + 0.75;

                    int rgComponents = (int)(sunDotProduct * 255 * ratio);

                    int textureComponent = (int)(sunDotProduct * 255 * textureFactor * ratio);

                    image[index] = ALPHA | textureComponent | (rgComponents << 8) | (rgComponents << 16);
                }
                else
                {
                    double ratio = (distance - VENUS_RADIUS) / VENUS_ATMOSPHERE;

                    int rgComponents = (int)((255 - ratio * 255) * sunDotProduct);

                    image[index] = ALPHA | (rgComponents << 8) | (rgComponents << 16);
                }
            }
        }
    }
}
