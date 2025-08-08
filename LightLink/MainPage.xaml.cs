using Camera.MAUI;
using LightLink.Common;

namespace LightLink
{
	public enum TransmitterType
	{
		Flash, Screen
	}
	public enum BitState
	{
		Off, On
	}

    public partial class MainPage : ContentPage
    {
		private bool isTransmitting = false;
		private const int OnBitDuration = 400;
		private const int OffBitDuration = 600;

		public MainPage()
		{
			InitializeComponent();

			cameraView.CamerasLoaded += CameraView_CamerasLoaded;
			cameraView.BarCodeDetectionEnabled = true;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
		}

		private void CameraView_CamerasLoaded(object sender, EventArgs e)
		{
			if (cameraView.Cameras.Count > 0)
			{
				cameraView.Camera = cameraView.Cameras.First();

				MainThread.BeginInvokeOnMainThread(async () =>
				{
					if (await cameraView.StartCameraAsync() == CameraResult.Success)
					{

					}
				});

			}
		}

		/// <summary>
		/// Handles the click event of the "Send Message" button.
		/// </summary>
		private async void OnSendClicked(object sender, EventArgs e)
		{
			if (isTransmitting)
			{
				await DisplayAlert("Busy", "A message is already being transmitted.", "OK");
				return;
			}

			string message = MessageEntry.Text;
			if (string.IsNullOrWhiteSpace(message))
			{
				await DisplayAlert("Error", "Please enter a message to send.", "OK");
				return;
			}

			// Convert the message to binary
			string binaryMessage = BinaryConverter.StringToBinary(message);
			BinaryOutputLabel.Text = $"Binary: {binaryMessage}";
			BinaryOutputLabel.IsVisible = true;

			// Start the transmission process
			await TransmitMessage(binaryMessage);
		}

		/// <summary>
		/// Transmits the binary message using flashlight or screen.
		/// </summary>
		/// <param name="binaryMessage">The binary string to transmit.</param>
		private async Task TransmitMessage(string binaryMessage)
		{
			isTransmitting = true;
			SendButton.IsEnabled = false;
			SendButton.Text = "Transmitting...";

			try
			{
				// Check if the device has a flashlight
				bool hasFlashlight = await HasFlashlightAsync();

				if (hasFlashlight)
				{
					await TransmitWithFlashlight(binaryMessage);
				}
				else
				{
					await DisplayAlert("No Flashlight", "This device doesn't have a flashlight. Using the screen instead.", "OK");
					await TransmitWithScreen(binaryMessage);
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Transmission Error", $"An error occurred: {ex.Message}", "OK");
			}
			finally
			{
				// Clean up and reset UI
				if (await HasFlashlightAsync())
				{
					await Flashlight.Default.TurnOffAsync();
				}
				FlashScreen.IsVisible = false;
				isTransmitting = false;
				SendButton.IsEnabled = true;
				SendButton.Text = "Send Message";
				BinaryOutputLabel.IsVisible = false;
			}
		}

		/// <summary>
		/// Checks for flashlight support safely.
		/// </summary>
		private async Task<bool> HasFlashlightAsync()
		{
			try
			{
				return await Flashlight.Default.IsSupportedAsync();
			}
			catch (FeatureNotSupportedException)
			{
				return false;
			}
			catch (PermissionException)
			{
				// This could happen if permissions are denied.
				return false;
			}
		}

		/// <summary>
		/// Flashes the binary message using the device's LED flashlight.
		/// </summary>
		private async Task TransmitWithFlashlight(string binaryMessage)
		{
			// A brief flash to signal the start of transmission
			await Flashlight.Default.TurnOnAsync();
			await Task.Delay(100);

			foreach (char bit in binaryMessage)
			{
				await TransmitMessage(TransmitterType.Screen, bit == '1' ? BitState.On : BitState.Off);
			}
		}

		/// <summary>
		/// Flashes the binary message using the device's screen.
		/// </summary>
		private async Task TransmitWithScreen(string binaryMessage)
		{
			FlashScreen.IsVisible = true;

			// A brief flash to signal the start
			FlashScreen.Color = Colors.White;
			await Task.Delay(100);

			foreach (char bit in binaryMessage)
			{
				await TransmitMessage(TransmitterType.Screen, bit == '1' ? BitState.On : BitState.Off);
			}
		}

		private async Task TransmitMessage(TransmitterType type, BitState state)
		{
			System.Diagnostics.Debug.WriteLine("Sending: " + state);

			switch(type)
			{
				case TransmitterType.Screen:
					{
						switch(state)
						{
							case BitState.On:
								{
									FlashScreen.Color = Colors.White;
									await Task.Delay(OnBitDuration);
								}
								break;

								case BitState.Off:
								{
									FlashScreen.Color = Colors.Black;
									await Task.Delay(OffBitDuration);
								}
								break;
						}
					}
					break;

				case TransmitterType.Flash:
					{
						switch (state)
						{
							case BitState.On:
								{
									await Flashlight.Default.TurnOnAsync();
									await Task.Delay(OnBitDuration);
								}
								break;

							case BitState.Off:
								{
									await Flashlight.Default.TurnOffAsync();
									await Task.Delay(OffBitDuration);
								}
								break;
						}
					}
					break;
			}
		}
	}
}
