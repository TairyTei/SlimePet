using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
   private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
   private IntPtr hWnd;
   private uint originalStyle;

   [DllImport("user32.dll")]
   public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

   [DllImport("user32.dll")]
   private static extern IntPtr GetActiveWindow();

   [DllImport("user32.dll")]
   private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

   [DllImport("user32.dll", SetLastError = true)]
   private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

   [DllImport("user32.dll")]
   private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

   [DllImport("user32.dll")]
   static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

   private struct Margins
   {
      public int cxLeftWidth;
      public int cxRightWidth;
      public int cyTopHeight;
      public int cyBottomHeight;
   }

   [DllImport("Dwmapi.dll")]
   private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins margins);

   private const int GWL_EXSTYLE = -20;

   private const uint WS_EX_LAYERED = 0x00080000;
   private const uint WS_EX_TRANSPARENT = 0x00000020;

   private const uint LWA_COLORKEY = 0x00000001;

   private const uint SWP_NOZORDER = 0x0004;
   private const uint SWP_NOACTIVATE = 0x0010;

   private static TransparentWindow instance;
   public static TransparentWindow Instance { get { return instance; } }

   void Awake()
   {
      instance = this;
   }
   private void Start()
   {
      //MessageBox(new IntPtr(0), "Hello world!", "CAPTION SDFOPSDIFOPSD", 0);
      if (!Application.isEditor)
      {
         hWnd = GetActiveWindow();

         originalStyle = (uint)GetWindowLong(hWnd, GWL_EXSTYLE);

         SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);

         ClickThrough = true;
         SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);

         Margins margins = new Margins { cxLeftWidth = -1 };
         DwmExtendFrameIntoClientArea(hWnd, ref margins);
         Application.runInBackground = true;
      }
   }
   private bool clickThrough = false;
   public bool ClickThrough
   {
      get { return clickThrough; }
      set
      {
         clickThrough = value;
         if (value)
         {
            SetWindowLong(hWnd, GWL_EXSTYLE, originalStyle | WS_EX_LAYERED);
            SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
         }
         else
         {
            SetWindowLong(hWnd, GWL_EXSTYLE, originalStyle);
            SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
         }
      }
   }
   public void MoveToMonitor(int monitorIndex)
   {
      var displays = Display.displays;

      // Проверяем, существует ли указанный монитор
      if (monitorIndex < 0 || monitorIndex >= displays.Length)
      {
         Debug.Log("Указанный монитор не существует.");
         return;
      }

      // Получаем указанный монитор
      var targetDisplay = displays[monitorIndex];

      // Вычисляем новые координаты для центрирования окна
      int newX = targetDisplay.renderingWidth;
      int newY = 0; // Устанавливаем Y в 0 для верхней части монитора

      Margins margins = new Margins { cxLeftWidth = 0, cxRightWidth = 0, cyTopHeight = 0, cyBottomHeight = 0 };
      DwmExtendFrameIntoClientArea(hWnd, ref margins);
      // Устанавливаем новое положение окна
      SetWindowPos(hWnd, IntPtr.Zero, newX, newY, (int)targetDisplay.systemWidth, (int)targetDisplay.systemHeight, SWP_NOZORDER | SWP_NOACTIVATE);
   }
}