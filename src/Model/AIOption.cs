using SwinGameSDK;
using static SwinGameSDK.SwinGame;
using System;
using System.Collections.Generic;

public enum AIOption
{
    /* <summary>
     *  Easy, total random shooting
     * </summary>
     */
    Easy,

    /* <summary>
     *  Medium, marks squares around hits
     * </summary>
     */
    Medium,

    /* <summary>
     *  As medium, but removes shots once it misses
     * </summary>
     */
    Hard
}
