import BaseProvider from "./BaseProvider";
import DataTypes from "./DataTypes";
import TextDataTypeConverter from "./dataTypesConverters/TextDataTypeConverter";
import LookupDataTypeConverter from "./dataTypesConverters/LookupDataTypeConverter";
import DateDataTypeConverter from "./dataTypesConverters/DateDataTypeConverter";
import NumberDataTypeConverter from "./dataTypesConverters/NumberDataTypeConverter";

const dataTypesConverters = [
    {
        dataType: DataTypes.DATETIME,
        converter: new DateDataTypeConverter()
    },
    {
        dataType: DataTypes.LOOKUP,
        converter: new LookupDataTypeConverter()
    },
    {
        dataType: DataTypes.NUMBER,
        converter: new NumberDataTypeConverter()
    },
    {
        dataType: DataTypes.TEXT,
        converter: new TextDataTypeConverter()
    }
];

class DataTypeConverterProvider extends BaseProvider {
    getConverter(dataType) {
        return this.getItemByFilter(item => item.dataType === dataType, `Converter for data type ${dataType} not found`)
            .converter;
    }
}

export default new DataTypeConverterProvider(dataTypesConverters);