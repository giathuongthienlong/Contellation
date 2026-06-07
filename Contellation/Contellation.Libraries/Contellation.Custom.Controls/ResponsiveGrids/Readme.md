# ResponsiveGrid - Contellation Custom Controls

**ResponsiveGrid** là control WPF mạnh mẽ, lấy cảm hứng từ hệ thống lưới Bootstrap, giúp tạo layout responsive dễ dàng trên WPF.


## Tính năng nổi bật

- Hỗ trợ 5 breakpoints: **XS, SM, MD, LG, XL**
- Span, Offset, Push, Pull theo từng breakpoint
- Responsive Visibility (`HiddenXS`, `HiddenSM`, ...)
- `ColumnGap` và `RowGap` linh hoạt
- Auto Row Height (tự động theo chiều cao lớn nhất của item trong hàng)
- Hiển thị Grid Lines + Breakpoint Lines tiện debug
- Hiệu suất tốt, code sạch

## Các Attached Properties chính

| Property                    | Kiểu   | Mô tả |
|-----------------------------|--------|-------|
| `XS`, `SM`, `MD`, `LG`, `XL` | `int`  | Số cột chiếm tại breakpoint tương ứng (giá trị từ 1 đến 12) |
| `Offset`                    | `int`  | Dịch chuyển item sang phải (offset) |
| `Push`                      | `int`  | Đẩy item sang phải |
| `Pull`                      | `int`  | Kéo item sang trái |
| `HiddenXS`                  | `bool` | Ẩn item khi màn hình < SM |
| `HiddenSM`                  | `bool` | Ẩn item khi màn hình trong khoảng SM |
| `HiddenMD`                  | `bool` | Ẩn item khi màn hình trong khoảng MD |
| `HiddenLG`                  | `bool` | Ẩn item khi màn hình ≥ LG |

## Dependency Properties

| Property          | Kiểu          | Mặc định     | Mô tả |
|-------------------|---------------|--------------|-------|
| `ShowGridLines`   | `bool`        | `false`      | Bật/tắt hiển thị grid lines để debug |
| `ColumnGap`       | `double`      | `0`          | Khoảng cách ngang giữa các cột |
| `RowGap`          | `double`      | `0`          | Khoảng cách dọc giữa các hàng |
| `BreakPoints`     | `BreakPoints` | `Default`    | Cấu hình các breakpoint (XS, SM, MD, LG, XL) |

ShowGridLines (bool)
ColumnGap (double)
RowGap (double)
BreakPoints

Lưu ý

Mặc định mỗi item có XS="12"
Nên đặt Height hoặc VerticalAlignment rõ ràng cho child khi cần
Bật ShowGridLines="True" khi đang phát triển để debug

## Cách sử dụng

```xml
Sử dụng BreakPoints tùy chỉnh
<ui:ResponsiveGrid BreakPoints="{x:Static rg:BreakPoints.Default}" ... >
Hoặc tạo custom:
BreakPoints customBp = new BreakPoints 
{ 
    SM = 640, 
    MD = 900, 
    LG = 1200 
};
```

### 1. Thêm namespace

```xml
xmlns:ui="clr-namespace:Contellation.Custom.Controls;assembly=Contellation.Custom"
```
### 2. Sử dụng ResponsiveGrid cơ bản
```xml 
<ui:ResponsiveGrid 
    ShowGridLines="True"
    ColumnGap="16"
    RowGap="20">

    <Border ui:ResponsiveGrid.XS="12" ui:ResponsiveGrid.SM="6" ui:ResponsiveGrid.MD="4" 
            Background="#FF6B6B" Height="120">
        <TextBlock Text="XS=12 SM=6 MD=4" Foreground="White" VerticalAlignment="Center" HorizontalAlignment="Center"/>
    </Border>

    <Border ui:ResponsiveGrid.XS="12" ui:ResponsiveGrid.SM="6" ui:ResponsiveGrid.MD="4" 
            Background="#4ECDC4" Height="120">
        <TextBlock Text="XS=12 SM=6 MD=4" Foreground="White" VerticalAlignment="Center" HorizontalAlignment="Center"/>
    </Border>

    <Border ui:ResponsiveGrid.XS="12" ui:ResponsiveGrid.MD="4" 
            Background="#45B7D1" Height="120">
        <TextBlock Text="XS=12 MD=4" Foreground="White" VerticalAlignment="Center" HorizontalAlignment="Center"/>
    </Border>
</ui:ResponsiveGrid>
```

### 3. Sử dụng Offset, Push, Pull

```xml

<ui:ResponsiveGrid ShowGridLines="True" ColumnGap="12" RowGap="12">
    <Border ui:ResponsiveGrid.XS="12" ui:ResponsiveGrid.MD="6" ui:ResponsiveGrid.Offset="2"
            Background="#96CEB4" Height="100">
        <TextBlock Text="Offset MD=2" ... />
    </Border>
</ui:ResponsiveGrid>

```


### 4. Responsive Visibility
```xml

<Border ui:ResponsiveGrid.XS="12" 
        ui:ResponsiveGrid.HiddenLG="True"
        Background="Orange" Height="80">
    <TextBlock Text="Ẩn trên LG trở lên" ... />
</Border>

<Border ui:ResponsiveGrid.XS="12" 
        ui:ResponsiveGrid.HiddenXS="True"
        Background="Purple" Height="80">
    <TextBlock Text="Chỉ hiện từ SM trở lên" ... />
</Border>

```