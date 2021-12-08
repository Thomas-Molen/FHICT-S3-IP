import { Icon } from '@iconify/react';
import { compressToBase64, decompressFromBase64 } from 'lz-string';
import { React, useEffect } from 'react';
import CanvasDraw from "sugarypineapple-react-canvas-draw";
import useState from 'react-usestateref';
import { UseFetchWrapper } from '../../helpers';
import './DrawingComponent.css';
import ReactTooltip from 'react-tooltip';

export function DrawingComponent({ adventurerId }) {
    const fetchWrapper = UseFetchWrapper();
    const [keyValue, setKeyValue] = useState(1);
    //drawingCanvas
    const [drawingCanvas, setDrawingCanvas] = useState();
    const [drawing, setDrawing] = useState("");


    useEffect(() => {
        fetchWrapper.get(`Adventurer/get-drawing/${adventurerId}`)
            .then((response) => {
                setDrawing(decompressFromBase64(response.drawing));
            })
    }, [])

    return (
        <>
            <div className="DrawingCanvas mb-5">
                <div className="row-fluid m-2">
                    <Icon icon="feather:save" color="#585858" width="30" className="pointer drawingOption me-2" data-tip="Save drawing"
                        onClick={() => {
                            var drawing = drawingCanvas.getSaveData();
                            setDrawing(drawing);
                            fetchWrapper.post(`Adventurer/save-drawing/${adventurerId}`, { drawing: compressToBase64(drawing) })
                                .then(() => {
                                    setDrawing(drawing);
                                })
                        }} />
                    <Icon icon="ci:trash-empty" color="#585858" width="30" className="pointer drawingOption me-2" data-tip="Clear drawing"
                        onClick={() => {
                            drawingCanvas.eraseAll();
                        }} />
                    <Icon icon="ic:baseline-undo" color="#585858" width="30" className="pointer drawingOption me-2" data-tip="Undo last stroke"
                        onClick={() => {
                            drawingCanvas.undo();
                        }} />
                    <Icon icon="mdi:reload-alert" color="#585858" width="30" className="pointer drawingOption" data-tip="Reload drawing field"
                        onClick={() => {
                            fetchWrapper.post(`Adventurer/save-drawing/${adventurerId}`, { drawing: compressToBase64(drawingCanvas.getSaveData()) })
                                .then(() => {
                                    setDrawing(drawingCanvas.getSaveData);
                                    setKeyValue(keyValue + 1);
                                })
                        }} />
                </div>
                <hr className="m-0" />
                <div className="d-flex">
                    <CanvasDraw
                        className="NotePadCanvas"
                        key={keyValue}
                        ref={canvasDraw => (setDrawingCanvas(canvasDraw))}
                        brushColor={"#585858"}
                        backgroundColor={"#252525"}
                        catenaryColor={"ffffff"}
                        hideGrid
                        enablePanAndZoom
                        mouseZoomFactor={0.001}
                        lazyRadius={0}
                        brushRadius={2}
                        canvasWidth={1000}
                        canvasHeight={360}
                        saveData={drawing}
                        immediateLoading={true} />
                </div>
            </div>
        </>
    )
}