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



Property | Kiểu | Mô tả
----------------------------------------------------------
XS, SM, MD, LG, XL | int | Số cột chiếm (1-12) 
Offset | int | Dịch chuyển sang phải
Push / Pull | int | Đẩy hoặc kéo item
HiddenXS ... HiddenLG | bool | Ẩn theo breakpoint
----------------------------------------------------------

Dependency Properties

ShowGridLines (bool)
ColumnGap (double)
RowGap (double)
BreakPoints

Lưu ý

Mặc định mỗi item có XS="12"
Nên đặt Height hoặc VerticalAlignment rõ ràng cho child khi cần
Bật ShowGridLines="True" khi đang phát triển để debug

## Cách sử dụng

Sử dụng BreakPoints tùy chỉnh
<ui:ResponsiveGrid BreakPoints="{x:Static rg:BreakPoints.Default}" ... >
Hoặc tạo custom:
BreakPoints customBp = new BreakPoints 
{ 
    SM = 640, 
    MD = 900, 
    LG = 1200 
};

### 1. Thêm namespace

```xml
xmlns:ui="clr-namespace:Contellation.Custom.Controls;assembly=Contellation.Custom.Controls"
xmlns:rg="clr-namespace:Contellation.Custom.Controls;assembly=Contellation.Custom.Controls"
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