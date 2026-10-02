using System;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using ECUMasterEditor.Core.Binary;

namespace ECUMasterEditor;

public partial class MainWindow : Window
{
    private EcuBinary? _currentBinary;
    private string? _currentFilePath;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenFile_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new()
        {
            Title = "Open ECU File",
            Filter = "ECU Files (*.bin;*.hex)|*.bin;*.hex|BIN Files (*.bin)|*.bin|Intel HEX Files (*.hex)|*.hex|All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            _currentBinary = EcuFileLoader.Load(dialog.FileName);
            _currentFilePath = dialog.FileName;

            FileNameText.Text = $"File: {Path.GetFileName(dialog.FileName)}";
            FileSizeText.Text = $"Size: {_currentBinary.Size:N0} bytes";
            FileFormatText.Text = $"Format: {Path.GetExtension(dialog.FileName).ToUpperInvariant()}";

            DisplayHex();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not load ECU file.\n\n{ex.Message}",
                "ECU Master Editor",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void DisplayHex()
    {
        if (_currentBinary == null)
            return;

        StringBuilder output = new();

        const int bytesPerLine = 16;

        for (int address = 0;
             address < _currentBinary.Data.Length;
             address += bytesPerLine)
        {
            output.Append($"{address:X8}  ");

            int remaining = Math.Min(
                bytesPerLine,
                _currentBinary.Data.Length - address);

            for (int i = 0; i < remaining; i++)
            {
                output.Append(
                    $"{_currentBinary.Data[address + i]:X2} ");
            }

            output.AppendLine();
        }

        HexViewer.Text = output.ToString();
    }

    private void SaveAs_Click(object sender, RoutedEventArgs e)
    {
        if (_currentBinary == null)
        {
            MessageBox.Show(
                "There is no ECU file loaded.",
                "ECU Master Editor",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return;
        }

        SaveFileDialog dialog = new()
        {
            Title = "Save ECU File",
            Filter = "BIN Files (*.bin)|*.bin",
            FileName = "modified.bin"
        };

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            File.WriteAllBytes(
                dialog.FileName,
                _currentBinary.Data);

            MessageBox.Show(
                "ECU file saved successfully.",
                "ECU Master Editor",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not save ECU file.\n\n{ex.Message}",
                "ECU Master Editor",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}