using System.Collections.Generic;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui.Platform
{
	// TODO: Make this class public in .NET11.0. Issue Link: https://github.com/dotnet/maui/issues/30205
	internal partial class MauiBorderAutomationPeer : FrameworkElementAutomationPeer, IInvokeProvider
	{
		internal MauiBorderAutomationPeer(Panel owner) : base(owner) { }

		// Control Type: Returns "Custom" when the border is interactive (has tap gestures / IsTabStop),
		// so screen readers announce it as actionable. Returns "Pane" for non-interactive borders.
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			if ((Owner as ContentPanel)?.IsTabStop == true)
			{
				return AutomationControlType.Custom;
			}

			return AutomationControlType.Pane;
		}

		// Class Name: Returns "Border" to match the semantic role and align with WinUI's BorderAutomationPeer.
		protected override string GetClassNameCore()
		{
			return "Border";
		}

		// Localized Control Type: When the border is interactive, screen readers announce it as "border"
		// instead of the generic "custom" text, giving users context about the element type.
		// TODO: Route through a .resw resource file for localization when MAUI's localization infra supports it.
		// Track: https://github.com/dotnet/maui/issues/30205
		protected override string GetLocalizedControlTypeCore()
		{
			if ((Owner as ContentPanel)?.IsTabStop == true)
			{
				return "border";
			}

			return base.GetLocalizedControlTypeCore();
		}

		// Pattern Support: Expose IInvokeProvider when the border is interactive (IsTabStop = true),
		// so UIA clients (Narrator, UI Automation frameworks) can programmatically activate the element.
		// This makes the interaction contract machine-readable, complementing GetAutomationControlTypeCore.
		protected override object GetPatternCore(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke &&
				(Owner as ContentPanel)?.IsTabStop == true)
			{
				return this;
			}

			return base.GetPatternCore(patternInterface);
		}

		// IInvokeProvider.Invoke: Programmatically fire the border's tap gesture recognizers,
		// following the same activation path as Enter/Space key handling in GesturePlatformManager.
		void IInvokeProvider.Invoke()
		{
			if (Owner is ContentPanel cp)
			{
				cp.KeyboardActivate?.Invoke();
			}
		}

		// Keyboard Focusable: Allows border to receive keyboard focus only when it has gesture recognizers (IsTabStop = true)
		protected override bool IsKeyboardFocusableCore() => (Owner as ContentPanel)?.IsTabStop == true;

		// Control View: Expose only when the Border is interactive (has tap gestures) or has an explicit AutomationId
		protected override bool IsControlElementCore() => IsInteractiveOrNamed();

		// Content View: Expose only when the Border is interactive (has tap gestures) or has an explicit AutomationId
		protected override bool IsContentElementCore() => IsInteractiveOrNamed();

		// Children: Filter out the rendering-only _borderPath (Path used for stroke/fill) so it does not
		// appear as a child peer in the UIA tree. Only the actual MAUI content child is exposed.
		// This mirrors the pattern used by MauiButtonAutomationPeer to suppress internal children.
		protected override IList<AutomationPeer>? GetChildrenCore()
		{
			if (Owner is not ContentPanel contentPanel)
				return base.GetChildrenCore();

			var allChildren = base.GetChildrenCore();
			if (allChildren is null)
				return null;

			var filtered = new List<AutomationPeer>();
			foreach (var peer in allChildren)
			{
				// Exclude the rendering-only border Path from the UIA tree
				if (peer is FrameworkElementAutomationPeer fePeer &&
					fePeer.Owner == contentPanel.BorderPath)
				{
					continue;
				}

				filtered.Add(peer);
			}

			return filtered.Count > 0 ? filtered : null;
		}

		// Gates automation exposure: Border appears in the control/content view only when it is interactive
		// (IsTabStop == true, set by GesturePlatformManager when tap gestures are present) or explicitly
		// named via AutomationId (opted-in for automation visibility by the developer).
		bool IsInteractiveOrNamed()
		{
			if (Owner is not ContentPanel contentPanel)
			{
				return false;
			}

			return contentPanel.IsTabStop || !string.IsNullOrEmpty(AutomationProperties.GetAutomationId(contentPanel));
		}
	}
}
