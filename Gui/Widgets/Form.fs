namespace Gui.Widgets


module Form =

    open Avalonia.FuncUI.DSL
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.Media

    open Utilities.Extensions
    open Gui

    let formElement name element : IView<StackPanel> =
        StackPanel.create
        <| [ StackPanel.orientation Orientation.Vertical
             StackPanel.margin Theme.spacing.medium
             StackPanel.spacing Theme.spacing.small
             StackPanel.children
             <| [ TextBlock.create [ TextBlock.text name ]
                  element ] ]

    let textItem
        (state: {| name: string
                   value: string
                   onSelected: string -> unit |})
        : IView<StackPanel> =

        formElement state.name
        <| TextBox.create [ TextBox.text state.value
                            TextBox.onTextChanged state.onSelected ]
        
    let multiline
        (state: {| name: string
                   value: string
                   onSelected: string -> unit |})
        : IView<StackPanel> =

        formElement state.name
        <| TextBox.create [ TextBox.acceptsReturn true
                            TextBox.textWrapping TextWrapping.Wrap
                            TextBox.text state.value
                            TextBox.onTextChanged state.onSelected ]

    let dropdownSelection
        (state: {| name: string
                   selected: 'a
                   onSelected: 'a -> unit |})
        : IView<StackPanel> =
            
        formElement state.name
        <| ComboBox.create [ ComboBox.dataItems (Seq.map DiscriminatedUnion.toString DiscriminatedUnion.allCases<'a>)
                             ComboBox.selectedItem (DiscriminatedUnion.toString state.selected)
                             ComboBox.onSelectedItemChanged (tryUnbox >> Option.iter state.onSelected) ]