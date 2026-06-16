# NotifyIcon - Contellation Custom Controls

**NotifyIcon** là control WPF chuyên nghiệp giúp hiển thị icon trong System Tray (khay hệ thống), hỗ trợ đầy đủ các tương tác chuột và ContextMenu.

---

## Tính năng nổi bật

- Hỗ trợ đầy đủ các sự kiện chuột (Left Click, Double Click, Right Click, Middle Click)
- Tích hợp ContextMenu WPF dễ dàng
- Hỗ trợ Icon từ Resource hoặc file
- Tooltip tùy chỉnh
- Focus MainWindow khi click trái (tùy chọn)
- Hỗ trợ MVVM qua Routed Events
- Dispose an toàn, tránh memory leak

## Các Dependency Properties chính

| Property           | Kiểu          | Mặc định     | Mô tả |
|--------------------|---------------|--------------|-------|
| `Icon`             | `ImageSource` | `null`       | Icon hiển thị trong tray |
| `TooltipText`      | `string`      | `""`         | Tooltip khi hover icon |
| `Menu`             | `ContextMenu` | `null`       | Context menu khi right click |
| `MenuFontSize`     | `double`      | `14`         | Kích thước font của menu |
| `FocusOnLeftClick` | `bool`        | `true`       | Focus MainWindow khi click trái |
| `MenuOnRightClick` | `bool`        | `true`       | Tự động mở menu khi right click |

## Routed Events
| Event               | Mô tả         |
|---------------------|---------------|
| `LeftClick`         | `Click trái` |
| `LeftDoubleClick`   | `Double click trái` | 
| `RightClick`        | `Click phải` | 
| `RightDoubleClick`  | `Double click phải` |
| `MiddleClick`       | `Click giữa`        |
| `MiddleDoubleClick` | `Double click giữa` |

## Cách sử dụng

### 1. Thêm namespace

```xml
xmlns:ui="clr-namespace:Contellation.Custom.Controls.TrayIcon;assembly=Contellation.Custom.Controls"
```

### 2. Sử dụng cơ bản

```xml
<ui:NotifyIcon 
    Icon="pack://application:,,,/Contellation;component/Assets/icon.ico"
    TooltipText="Contellation Application"
    FocusOnLeftClick="True"
    MenuOnRightClick="True">

    <ui:NotifyIcon.Menu>
        <ContextMenu>
            <MenuItem Header="Mở ứng dụng" Click="OnOpenClick"/>
            <MenuItem Header="Cài đặt" Click="OnSettingsClick"/>
            <Separator/>
            <MenuItem Header="Thoát" Click="OnExitClick"/>
        </ContextMenu>
    </ui:NotifyIcon.Menu>
</ui:NotifyIcon>
```

### 3. Dùng MVVM (Binding)
```xml

<ui:NotifyIcon 
    Icon="{Binding TrayIconPath}"
    TooltipText="{Binding TrayTooltip}"
    LeftClickCommand="{Binding ShowWindowCommand}"   <!-- Nếu dùng Command -->
    MenuOnRightClick="True">

    <ui:NotifyIcon.Menu>
        <ContextMenu ItemsSource="{Binding TrayMenuItems}"/>
    </tray:NotifyIcon.Menu>
</ui:NotifyIcon>

```

### Lưu ý: quan trọng

Nên đặt NotifyIcon trong Window.Resources hoặc một container không hiển thị (ví dụ Grid với Visibility="Collapsed").
Luôn gọi Dispose() khi đóng ứng dụng để dọn dẹp tài nguyên.
Icon nên dùng file .ico để hiển thị tốt nhất.
Nếu dùng MVVM, nên tạo Command wrapper cho các RoutedEvent nếu cần.
